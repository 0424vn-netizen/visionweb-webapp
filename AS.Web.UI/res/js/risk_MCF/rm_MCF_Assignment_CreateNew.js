function doOpenSubPopup(url, width, height) {
    var scrW = getScreenWidth();
    var scrH = getScreenHeight();
    var sizeRate = 0.70;
    if (width == null)
        width = scrW * sizeRate;
    if (height == null)
        height = scrH * sizeRate;


    parent.master_closeModalEvent = function () {
        if (rm_Assignment_CreateNew_isDetectionQueueAssignment_Client.toLowerCase() == "true") {
            filter_closeModalEvent(parent._modalID, parent._clientIDbtn);
            parameter_closeModalEvent(parent._modalID);
        }
        info_closeModalEvent(parent._modalID);
        parent.master_closeModalEvent = null;
    }
    return parent.ShowPopupModalChild(1, url, width, height);
}
parent._saveConfirm = false;
parent._isDuplicate = false;
parent._modalID = '';
parent.setModalID = function (id) {
    parent._modalID = id;
}

parent._clientIDbtn = ''
parent.setClientIDbtn = function (id) {
    parent._clientIDbtn = id;
}

parent.setSaveConfirm = function (value) {
    parent._saveConfirm = value;
}
parent.getSaveConfirm = function () {
    return parent._saveConfirm;
}

parent.saveAssignment = function () {
    var btn = document.getElementById(rm_Assignment_CreateNew_uxSaveAssignment);
    btn.click();
}

function checkDuplicateAssignmentName(isDuplicate) {

    if (isDuplicate == 1) {
        return false;
    }
    else {
        //count the number of selected merchant and get confirm from user 
        //before processing saving process
        parent.setSaveConfirm(true);
        if (rm_Assignment_CreateNew_isDetectionQueueAssignment_Client.toLowerCase() == "true") {
            btnCountClick();
        }
        else {
            parent.saveAssignment();
        }
        return false;
    }
}

/****SHOW/HIDE FINISH BUTTON*****/
function showFinishButton() {
    var isShow = true;
    $("#uxAssignmentParamsGrid").find("div[id*='param']").each(function () {
        if ($("#uxAssignmentParamsGrid").find("div[id='" + $(this).prop("id") + "']").length > 1) {
            $("#uxAssignmentParamsGrid").find("div[id='" + $(this).prop("id") + "']").each(function () {
                if ($(this).parents("div[class*='group']").children("div[id*='param']").length == 1) {
                    isShow = false;
                    $(this).find("span[id*='uxStandAloneIcon']").removeClass("hide");
                } else {
                    $(this).find("span[id*='uxStandAloneIcon']").addClass("hide");
                }
            });
        } else {
            if (!$(this).find("span[id*='uxStandAloneIcon']").hasClass("hide")) {
                $(this).find("span[id*='uxStandAloneIcon']").addClass("hide");
            }
        }
    });
    if (isShow)
        $("#" + rm_Assignment_CreateNew_uxSave).removeAttr("disabled");
    else
        $("#" + rm_Assignment_CreateNew_uxSave).attr("disabled", "disabled");
    //Check show/hide title
    if (typeof (Risk_Assignment_Parameters_rdMatchAll) !== "undefined" && $("#" + Risk_Assignment_Parameters_rdMatchAll).prop("checked") == false) {
        //$("#divGroupTitle").text(Risk_Assignment_Parameters_js_group);
        $("#divGroupTitle").removeClass("hide");
    } else {
        //$("#divGroupTitle").text("");
        $("#divGroupTitle").addClass("hide");
    }
    if (rm_Assignment_CreateNew_isDetectionQueueAssignment_Client.toLowerCase() == "true") {
        //Reset timeout
        resetTimeout();
    }
}

jQuery(document).ready(function () {
    showFinishButton();
    if (rm_Assignment_CreateNew_isDetectionQueueAssignment_Client.toLowerCase() == "true") {
        showGroupButton();
    }
    if (typeof (Risk_Assignment_Parameters_rdMatchAll) !== "undefined" && $("#" + Risk_Assignment_Parameters_rdMatchAll).prop("checked")) {
        $("#uxAssignmentParamsGrid").addClass("hide-checkbox");
    }
    var width = $(".modal-xxl").width();
    var height = $("#fixedPanel").height() - 1;
    var bgColor = $(".body-modal").css("background-color");
    $("#fixedHeight").css("height", height + "px");
    $("#fixedPanel").css({ "bottom": "0px", "position": "fixed", "width": width - 40 + "px", "z-index": 1000, "background": "#fff", "left": "0px", "right": "0px", "margin": "auto" });
    $("#fixedFooter").css({ "height": "20px", "background-color": bgColor });
    $("#fixedContain").css({ "border-left": "1px solid #ccc", "border-right": "1px solid #ccc", "border-bottom": "1px solid #ccc", "padding-right": "17px", "padding-bottom": "24px" });
    $("#fixedShadowBox").css({ "box-shadow": "#DAD3D3 0px -2px 1px", "width": "1030px", "height": "1px", "left": "25px" });
    getTrackingData("#uxAssignmentInfoGrid, #uxDivCustomView");
});

function ValidateCriteriaFilter() {
    let result = false;
    if (rm_Assignment_CreateNew_isDetectionQueueAssignment_Client.toLowerCase() == "true") {
        if (!saveParameters())
            return false;
    }
    let agnInfoResult = ValidateAssignmentGeneralInformation();
    let agnFilterResult = true;
    let agnParamResult = true;
    
    if (rm_Assignment_CreateNew_isDetectionQueueAssignment_Client.toLowerCase() == "true") {
        agnFilterResult = ValidateAssignmentFilters();
        agnParamResult = ValidateAssignmentParameters();        
    }

    if (!agnInfoResult) {
        moveTo('#markupAssinfo');
    }
    else if (!agnFilterResult) {
        moveTo('#markupFilter');
    }
    else if (!agnParamResult) {
        moveTo('#markupParam');
    }
    result = agnInfoResult && agnFilterResult && agnParamResult;
    if (result && !rm_Assignment_CreateNew_IsCreateNewAssignment) {
        //check confirm 
        let tempaudit = $("#" + rm_Assignment_CreateNew_uxAssignmentTrackingJsonTemp).val();
        if (tempaudit) {
            $("#" + rm_Assignment_CreateNew_uxAssignmentTrackingJson).val(tempaudit);
            assignmentTrackingObj = JSON.parse(tempaudit);
        }
        else {
            $("#" + rm_Assignment_CreateNew_uxAssignmentTrackingJsonTemp).val($("#" + rm_Assignment_CreateNew_uxAssignmentTrackingJson).val());
        }
        //Update new value for tracking assignment
        TrackingAssignment(true, true);
    }
    return result;
}

function moveTo(id) {
    $('html, body').animate({
        scrollTop: $(id).offset().top + 'px'
    }, 'fast');
}

function LoadAssignmentFilters() {
    $get(rm_Assignment_CreateNew_uxLoadAssignmentFilters).click();
    AdjustModalSize();
}

function LoadAssignmentParameters() {
    $get(rm_Assignment_CreateNew_uxLoadAssignmentParameters).click();
    AdjustModalSize();
    displayGridParams();
}

$(document).ready(function () {
    if (rm_Assignment_CreateNew_IsFirstLoad) {
        setTimeout("LoadAssignmentFilters();", 100);
    }

    let uxCustomViewReadOnly_Control = $find(uxCustomViewReadOnly_ClientID);
    if (uxCustomViewReadOnly_Control) {
        uxCustomViewReadOnly_Control.disable();
    }

});

//-------ASSIGNMENT TRACKING----------//
function onChangeAssignmentType(assignmentType) {
    if (assignmentType === 1) { //Work Queue
        TrackingAssignment(false, true);
    }
}

function TrackingAssignment(isSave, isFull) { //isSave = true, the value will update to NewValues otherwise update OldValues
    if (!rm_Assignment_CreateNew_IsCreateNewAssignment) {
        let idFind = "#uxAssignmentFiltersGrid, #ucParameterType, #ucParameterRiskScore";
        if (isFull)
            idFind = "#uxAssignmentInfoGrid, #uxDivCustomView, " + idFind;

        getTrackingData(idFind, isSave);

        // Combine Parameter to format, Ex: "P1,P3,[P8, P4],P7"
        let paramsCombine = "";
        let groups = $("#uxParameterContent .groups");
        let params = $("#uxParameterContent").find("div[id^=param]");
        let actions = $("#" + rm_Assignment_CreateNew_uxParamActions).val();

        if (params.length === 0 && actions === "") {
            removeParamterItem();
        } else {
            if (actions === "")
                actions = "AddParameter";
            if (groups.length === params.length) { //Case no group
                var paramArr = [];
                params.each(function () {
                    paramArr.push(($(this).attr("id")).split("_")[1].toUpperCase());
                });
                paramsCombine = paramArr.join(", ");
            } else {
                paramArr = [];
                groups.each(function () { //Case params in group
                    var paramInGroup = $(this).find("div[id^=param]");
                    if (paramInGroup.length === 1) {
                        paramArr.push(($(paramInGroup).attr("id")).split("_")[1].toUpperCase());
                    } else {
                        var paramInGroupArr = [];
                        paramInGroup.each(function () {

                            paramInGroupArr.push(($(this).attr("id")).split("_")[1].toUpperCase());
                        })
                        paramArr.push("[" + paramInGroupArr.join(" + ") + "]");
                    }
                });
                paramsCombine = paramArr.join(", ");
            }
            if (actions.split(",").length > 1) {
                actions = rm_Assignment_CreateNew_Multiple;
            }
            updateValueOfTrackingAssignment("Parameters", actions, paramsCombine, isSave);
        }

        // Add Indicator, Threshold of Paramter

        //In case remove Parameters, Remove all Indicator, Threshold items and re-scan
        removeIndicatorThreshold();
        if (params.length > 0) {
            var oldParams = getOldParameters();
            $("#uxAssignmentParamsGrid").find("*[tracking-key]").each(function () {
                var key = $(this).attr("tracking-key");
                var param = key.split("_")[0];
                if (paramsCombine.indexOf(param) > -1 &&
                    oldParams.indexOf(param) > -1) { //Skip params are added new
                    var refer = $(this).attr("tracking-refer");
                    var trackingText = $(this).attr("tracking-text");
                    var strText = $(this).text() == 'N/A' ? "" : $(this).text();
                    var value = $find(this.id) == null ? strText : parseToNegativeNumber($find(this.id).get_displayValue());

                    if (refer) {
                        $("#uxAssignmentParamsGrid").find("input[id$=" + refer + "]").each(function () {
                            if ($(this).attr("refer-key") === key) { //refer-key: in the case have From-To, Low-High
                                value = value + " - " + $(this).val(); //From - To
                            }
                        });
                    }
                    updateValueOfTrackingAssignment(key, trackingText, value, isSave);
                }
            });
        }
        if (isSave)
            processEcommerce();
        //optimizeTrackingObj();
        //Keep Old Value when change Assignment Type
        if ($("#" + rm_Assignment_CreateNew_uxAssignmentTrackingJson).val()) {
            let finalObj = JSON.parse($("#" + rm_Assignment_CreateNew_uxAssignmentTrackingJson).val());
            for (var i = 0; i < finalObj.length; i++) {
                let obj = finalObj[i];
                let idx = findKeyIndex(obj.Key);
                if (idx > -1 && (obj.Key !== "ECommerceBetweenFrom" && obj.Key !== "KeyedBetweenFrom" && obj.Key !== "SwipedBetweenFrom")) {
                    assignmentTrackingObj[idx].OldValues = obj.OldValues;
                }
            }
        }
        optimizeTrackingObj(isSave);

        $("#" + rm_Assignment_CreateNew_uxAssignmentTrackingJson).val(JSON.stringify(assignmentTrackingObj));
    }
}
//Remove all Indicator and Threshold info from Tracking object
function removeIndicatorThreshold() {
    var obj = [];
    for (var i = 0; i < assignmentTrackingObj.length; i++) {
        var key = assignmentTrackingObj[i].Key;
        if (key.indexOf("_FAI") === -1
            && key.indexOf("_FAT") === -1
            && key.indexOf("_RAI") === -1
            && key.indexOf("_RAT") === -1) {
            obj.push(assignmentTrackingObj[i]);
        }
    }
    assignmentTrackingObj = obj;
}
//Remove item has "Paramters" key
function removeParamterItem() {
    var obj = [];
    for (var i = 0; i < assignmentTrackingObj.length; i++) {
        if (assignmentTrackingObj[i].Key !== "Parameters")
            obj.push(assignmentTrackingObj[i]);
    }
    assignmentTrackingObj = obj;
}
//Get OldValues of item "Parameters"
function getOldParameters() {
    for (var i = 0; i < assignmentTrackingObj.length; i++) {
        if (assignmentTrackingObj[i].Key === "Parameters") {
            return assignmentTrackingObj[i].OldValues;
        }
    }
    return "";
}
//Change "(100)" -> -100
function parseToNegativeNumber(number) {
    if (number.indexOf(")") > -1 && number.indexOf("(") > -1)
        return "-" + number.replace("(", "").replace(")", "");
    return number;
}
//Find item index by Key tracking
function findKeyIndex(key) {
    for (var i = 0; i < assignmentTrackingObj.length; i++) {
        if (assignmentTrackingObj[i].Key === key)
            return i;
    }
    return -1;
}
//Remove item have the same OldValues and NewValues
function optimizeTrackingObj(isSave) {
    var obj = [];
    for (var i = 0; i < assignmentTrackingObj.length; i++) {
        if (assignmentTrackingObj[i].OldValues !== assignmentTrackingObj[i].NewValues) {
            if ((assignmentTrackingObj[i].OldValues.toString().indexOf("#-IE-#") > 0 ||
                assignmentTrackingObj[i].NewValues.toString().indexOf("#-IE-#") > 0) && isSave) {
                if (assignmentTrackingObj[i].OldValues != "") {
                    var strOld = assignmentTrackingObj[i].OldValues.split("#-IE-#");
                    assignmentTrackingObj[i].OldOptionID = strOld[0].trim() == "I" ? "1" : strOld[0].trim() == "E" ? "2" : "";
                    assignmentTrackingObj[i].OldValues = strOld[1];
                }

                if (assignmentTrackingObj[i].NewValues != "") {
                    var strNew = assignmentTrackingObj[i].NewValues.split("#-IE-#");
                    assignmentTrackingObj[i].NewOptionID = strNew[0].trim() == "I" ? "1" : strNew[0].trim() == "E" ? "2" : "";
                    assignmentTrackingObj[i].NewValues = strNew[1];
                }
            }

            obj.push(assignmentTrackingObj[i]);
        }
    }
    assignmentTrackingObj = obj;
}
function GetValueOfControl(control, obj) {
    var id = $(obj).attr("id");
    if (!id) {
        var refer = $(obj).attr("tracking-refer");
        id = $("*[id$=" + refer + "]").prop("id");
    }
    if ($(id).closest("div").css("display") !== "none" &&
        !$(id).closest("div").hasClass("hide")) {
        switch (control) {
            case "text":
                if ($(obj).attr("disabled") === "disabled")
                    return "";
                return $(obj).val();
            case "combobox":
                if ($find(id) && $find(id)._enabled && $find(id).get_selectedItem() != null)
                    return $find(id).get_selectedItem().get_value();
                return "";
            case "comboboxIndex":
                return $find(id).get_selectedItem().get_index();
            case "checkbox":
            case "radio":
                return $(obj).find("input").prop("checked") == true ? "1" : "0";
            case "date":
                return $find(id).get_dateInput().get_selectedDate().format("MM/dd/yyyy");
            case "hierarchy":
                var hierarchyText = $(obj).attr("tracking-value");
                return hierarchyText !== "N/A" ? hierarchyText : "";
            case "radio-text":
            default:
                return $(obj).text() === "N/A" ? "" : ($(obj).attr("tracking-more") !== undefined ? $(obj).attr("tracking-more") : $(obj).text());
        }
    }
}
function updateValueOfTrackingAssignment(key, text, value, isSave) {
    key = key.replace(/ /g, "");
    text = text.replace(/ /g, "");
    value = typeof (value) === "string" ? value.trim() : value;
    var idx = findKeyIndex(key);
    if (idx !== -1) {
        if (isSave) {
            assignmentTrackingObj[idx].NewValues = value;
            assignmentTrackingObj[idx].OldValues = assignmentTrackingObj[idx].OldValues;
            assignmentTrackingObj[idx].KeyLang = key === text ? assignmentTrackingObj[idx].KeyLang : text;
        }
        else {
            assignmentTrackingObj[idx].OldValues = value;
        }
    }
    else {
        var defaultOldValues = typeof (value) === "boolean" ? false : "";
        var obj = { Key: "", KeyLang: "", OldValues: defaultOldValues, NewValues: "" };
        obj.KeyLang = text;
        obj.Key = key;
        if (isSave) {
            obj.NewValues = value;
        } else {
            obj.OldValues = value;
        }
        assignmentTrackingObj.push(obj);
    }
}
function processEcommerce() {
    processFilterE("ECommerce");
    processFilterE("Keyed");
    processFilterE("Swiped");
}
function processFilterE(key) {
    var indexType = findKeyIndex(key + "Type");
    if (indexType > -1) {
        if (assignmentTrackingObj[indexType].NewValues === 1 || assignmentTrackingObj[indexType].NewValues === 2) {
            var indexBetweenFrom = findKeyIndex(key + "BetweenFrom");
            if (indexBetweenFrom > -1) {
                assignmentTrackingObj[indexBetweenFrom].Key = assignmentTrackingObj[indexType].NewValues === 1 ? key + "GreaterThan" : key + "LessThan";
                assignmentTrackingObj[indexBetweenFrom].KeyLang = assignmentTrackingObj[indexType].NewValues === 1 ? key + "GreaterThan" : key + "LessThan";
                if (assignmentTrackingObj[indexType].NewValues !== assignmentTrackingObj[indexType].OldValues)
                    assignmentTrackingObj[indexBetweenFrom].OldValues = "";
            }
            else {
                assignmentTrackingObj.push({
                    Key: assignmentTrackingObj[indexType].NewValues === 1 ? key + "GreaterThan" : key + "LessThan",
                    KeyLang: assignmentTrackingObj[indexType].NewValues === 1 ? key + "GreaterThan" : key + "LessThan",
                    OldValues: "", NewValues: $("input[tracking-key='" + key + "BetweenFrom']").val()
                });
            }
            var indexTo = findKeyIndex(key + "BetweenTo");
            if (indexTo > -1)
                assignmentTrackingObj.splice(indexTo, 1);
        }
        else {
            var indexFrom = findKeyIndex(key + "BetweenFrom");
            if (indexFrom < 0) {
                assignmentTrackingObj.push({
                    Key: key + "BetweenFrom",
                    KeyLang: key + "BetweenFrom", OldValues: "", NewValues: $("input[tracking-key='" + key + "BetweenFrom']").val()
                });
            }
            else if (assignmentTrackingObj[indexType].OldValues !== assignmentTrackingObj[indexType].NewValues
                && assignmentTrackingObj[indexType].NewValues !== "") {
                assignmentTrackingObj[indexFrom].OldValues = "";
            }
            var indexToData = findKeyIndex(key + "BetweenTo");
            if (indexToData < 0) {
                assignmentTrackingObj.push({
                    Key: key + "BetweenTo",
                    KeyLang: key + "BetweenTo", OldValues: "", NewValues: $("input[tracking-key='" + key + "BetweenTo']").val()
                });
            }
        }
        assignmentTrackingObj.splice(indexType, 1);
    }
}
function getTrackingData(idFind, isSave) {
    $(idFind).find("*[tracking-key]").each(function () {
        var key = $(this).attr("tracking-key"); //tracking-key: add to element need to tracking
        var type = $(this).attr("tracking-type") ? $(this).attr("tracking-type") : $(this).attr("type"); //tracking-type: textbox, combobox, radio, checkbox...
        var required = $(this).attr("tracking-required"); //tracking-required: with radio, checkbox uncheck but still need to keep false value
        if (type === "radio" || type === "checkbox" || type === "radio-text") {
            var isChecked = $(this).find("input").prop("checked");
            var isRefer = $(this).attr("tracking-refer") ? true : false; //tracking-refer: when radio, checkbox are checked but want to get value of another element
            var referType = $(this).attr("tracking-refer-type"); //tracking-refer-type: textbox, combobox...
            if (isChecked || required) {
                var value = isRefer ? GetValueOfControl(referType, this) : GetValueOfControl(type, this);
                updateValueOfTrackingAssignment(key, key, value, isSave);
            }
        } else {
            updateValueOfTrackingAssignment(key, key, GetValueOfControl(type, this), isSave);
        }
    });
}
function showAuditCreateNew() {
    var parameter = $get(rm_Assignment_CreateNew_AuditLink).value;
    url = rootURL + "risk_MCF/rm_MCF_Assignment_AuditReport.aspx?" + parameter;
    openPopupWindow(url, "window2");
}
//-----END ASSIGNMENT TRACKING------