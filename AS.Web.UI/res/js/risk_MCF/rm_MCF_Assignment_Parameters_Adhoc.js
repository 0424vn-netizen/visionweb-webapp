var ROW_CARD_TEMPLATE = "<div runat='server' id='divGroup' class='row-card'>"
  + "<div style='width: 34px' class='item w-checkbox j-center'>"
  + "<input id='chkGroup' type='checkbox' onclick='onCheckValidGroup(this);'>";
var GROUP_TEMPLATE = "</div><div class='groups'>";
function parameter_Add(modal) {
  saveParameters(modal);
  return false;
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
        url = rootURL + "Risk_MCF/rm_MCF_AdhocCreate.aspx/GetTransactionCode";
      else
        url = rootURL + "Risk_MCF/rm_MCF_Assignment_CreateNew.aspx/GetTransactionCode";
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
  var chk = $get(Risk_Assignment_Parameters_chkRiskScore);
  if (chk != null && chk.checked) {
    return true;
  }
  return false;
}

function ValidateAssignmentParameters() {
  //Validate riskscore
  //var itemsRepeater = parseInt(document.getElementById(Risk_Assignment_Parameters_CountItemRepeater).value);
  var itemsRepeater = $("#uxParameterContent").find("div[id*='param']").length;
  var validRC = validateRiskScoreValues();
  //var validRC = setTimeout("validateRiskScoreValues();", 300);
  if (!validRC)
    return false;
  var isChecked = validateRiskScoreCheck();
  //var isChecked = setTimeout("validateRiskScoreCheck();", 300);
  if (!isChecked) {
    //Validate parameters
    if (itemsRepeater == 0) {
      alert(Risk_Assignment_Parameters_js_msg1);
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
    $("#ucGroup").addClass("hide");
    $("#uxAssignmentParamsGrid").addClass("hide-checkbox");
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
  }
  return false;
}

/*****HELPER METHODS****/
function saveParameters(modal) {
  var actionUrl = "";
  if (isAdhoc())
    actionUrl = rootURL + "Risk_MCF/rm_MCF_AdhocCreate.aspx/SaveParametersToStagging";
  else
    actionUrl = rootURL + "Risk_MCF/rm_MCF_Assignment_CreateNew.aspx/SaveParametersToStagging";
  var primaryID = $("#" + Risk_Assignment_Parameters_uxPrimaryID_ClientID).val();
  var modeID = $("#" + Risk_Assignment_Parameters_uxMode_ClientID).val();
  var params = buildJson();
  var postData = { mode: modeID, assignmentID: primaryID, parameters: params };
  postServer(actionUrl, postData, function (result) {
    if (modal) {
      doOpenSubPopup(modal + Risk_Assignment_Parameters_QueryString, 'auto');
    }
  });
}

/******BUILD JSON DATA*******/
function buildJson() {
  var data = [];
  var groups = $("#uxParameterContent").find("div[id*='divGroup']");
  var groupId = 1;
  if (groups.length > 0) {
    groups.each(function () {
      $(this).find("div[id*='param']").each(function () {
        data.push(getJsonItem(groupId, $(this)));
      });
      groupId++;
    })
  }
  return data;
}

/******GET JSON DATA OF ITEM*******/
function getJsonItem(groupId, paramObj) {
  var isSourceParam = 0;
  var isParameterValueVisible = false;
  var thresholdLow = thresholdHigh = threshold = parameterValue = from = to = "0";
  var parameterPrecision = isThresholdNegative = isIndicatorNagative = parameterThresholdType = "0";
  if (paramObj.attr("data-is-source") == "true") {
    if (paramObj.find("input[id*='txtParameterValue']").length > 0) {
      isParameterValueVisible = true;
    }
    isSourceParam = 1;
    thresholdLow = paramObj.find("input[id*='txtThresholdLow']").val();
    thresholdHigh = paramObj.find("input[id*='txtThresholdHigh']").val();
    threshold = paramObj.find("input[id*='txtThreshold']").val();
    parameterValue = paramObj.find("input[id*='txtParameterValue']").val();
    from = paramObj.find("input[id*='txtFrom']").val();
    to = paramObj.find("input[id*='txtTo']").val();

    parameterKey = paramObj.find("input[id*='uxCriteriaValueString']").val();
    parameterPrecision = paramObj.find("input[id*='uxParameterPrecision']").val();
    isThresholdNegative = paramObj.find("input[id*='uxIsThresholdNegative']").val();
    isIndicatorNagative = paramObj.find("input[id*='uxIsIndicatorNagative']").val();
    parameterThresholdType = paramObj.find("input[id*='uxParameterThresholdType']").val();
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
    "IsParameterValueVisible": isParameterValueVisible
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
    url = rootURL + "Risk_MCF/rm_MCF_AdhocCreate.aspx/ResetTimeOut";
  else
    url = rootURL + "Risk_MCF/rm_MCF_Assignment_CreateNew.aspx/ResetTimeOut";
  postServer(url, data, null);
}
