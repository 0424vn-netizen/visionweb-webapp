function OpenInstanceWindow(url, name) {
    var width = screen.availWidth * 0.9;
    var height = screen.availHeight * 0.8;
    var left = (screen.availWidth / 2) - (width / 2);
    var top = (screen.availHeight / 2) - (height / 2);
    var pwin = window.open(url, "_blank", "scrollbars=1,menubar=0,toolbar=0,resizable=1,location=0,width=" + width.toString() + ",height=" + height.toString() + ",left=" + left.toString() + ",top=" + top.toString(), true);
    if (pwin != null) {
        pwin.focus();
    }
}

function MerchantNumberClick() {
    $get(rm_DQNextQReportPopup_btnClickMerchantNumber).click();
}

function ReloadParent(url) {
    window.opener.location.href = url;
}

var isAllowClickNext;
$(document).ready(function () {
    doPreloadMerchant();
    doInitialNextButton();
    keepSessionActive();
});

//$(window).bind("load", function () {
//    doPreloadMerchant();
//});

function doInitialNextButton() {
    isAllowClickNext = true;
    $('#' + rm_DQNextQReportPopup_uxNext).removeAttr('disabled');
    $('#' + rm_DQNextQReportPopup_uxNext).click(function (e) { return DisableNextQueueButton(); });
}
function DisableNextQueueButton() {
    // Only allow server submission with the first click
    if (isAllowClickNext) {
        isAllowClickNext = false;
        setTimeout(function () {
            $('#' + rm_DQNextQReportPopup_uxNext).blur();
            $('#' + rm_DQNextQReportPopup_uxNext).attr('disabled', 'disabled');
        }, 100);
        return true;
    }
    return false;
}

var preloadFailedAttemp = 0;
/*Reload merchant*/
function doPreloadMerchant() {
    //PageMethods.PreloadMerchant("", doPreloadSuccess, doPreloadFailure);
    var data = {};
    data["assignmentID"] = rm_DQNextQReportPopup_AssignmentID;
    data["reportDate"] = rm_DQNextQReportPopup_ReportDate;
    data["orderBy"] = rm_DQNextQReportPopup_OrderBy;
    data["applyFilterId"] = rm_DQNextQReportPopup_ApplyFilterId;

    var url = "rm_MCF_DQNextQWebMethod.aspx/PreloadMerchant";
    $.ajax({
        type: "POST",
        url: url,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data),
        //success: function (response) {
        //    //doPreloadSuccess(response["d"]);
        //    // allow the button to be clicked
        //},
        error: function (result) {
            doPreloadFailure(result);
        }
    });
}

//function doPreloadSuccess(data) { 
//    if (data != 'end' && parseInt(data) != rm_NextQueueModal_LENGHT_OF_QUEUE) {
//        doPreloadMerchant();
//    }
//}

function doPreloadFailure(error) {
    preloadFailedAttemp++;
    //parent.checkSession();
    if (preloadFailedAttemp <= 3) {
        doPreloadMerchant();
    }
}

//Refresh "rm_MerchantWorkedReport.aspx" page when close child window
var isRefresh = true;
function doAssign() {
    isRefresh = false;
}
window.onunload = function () {
    if (window.opener.location.href.indexOf("rm_MCF_MerchantWorkedReport.aspx") > 0 && isRefresh) {
        window.opener.location.reload();
    }
};
function keepSessionActive() {
    setTimeout("UpdateSessionActive()", 120000); //2 minutes - 120000
}
var updateFailedAttemp = 2;
function UpdateSessionActive() {
    var url = "rm_MCF_DQNextQReportPopup.aspx/KeepSessionActive";
    $.ajax({
        type: "POST",
        url: url,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            keepSessionActive();
        },
        error: function (result) {
            if (updateFailedAttemp > 0)
                keepSessionActive();
            updateFailedAttemp--;
        }
    });
}

function PageChanged(index) {
    var url = "rm_MCF_DQNextQReportPopup.aspx/GetNQTransactions"; // document.URL.replace('#', '') + '/CheckSSOUser';
    $("#uxProgress").show();
    $.ajax({
        type: "POST",
        url: url,
        data: '{"pageIndex":"' + index + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#uxProgress").hide();
            var data = result.d[0];
            var pager = result.d[1];

            $("#transactionTbl tr:not(:first)").remove();
            $("#transactionTbl").append(data);

            $("#" + pagerID).html(pager);

            $('html, body').animate({
                scrollTop: $('#divTransaction').offset().top
            });
        },
        error: function (result) {
            $("#uxProgress").hide();
            console.log(result);
        }
    });
}

function PageChangedCB(index) {
    var url = "rm_MCF_DQNextQReportPopup.aspx/GetNQChargebacks";
    $("#uxProgress").show();
    $.ajax({
        type: "POST",
        url: url,
        data: '{"pageIndex":"' + index + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#uxProgress").hide();
            var data = result.d[0];
            var pager = result.d[1];

            $("#chargebackTbl tr:not(:first)").remove();
            $("#chargebackTbl").append(data);

            $("#" + pagerIDCB).html(pager);
            $('html, body').animate({
                scrollTop: $('#divChargeback').offset().top
            });
        },
        error: function (result) {
            $("#uxProgress").hide();
            console.log(result);
        }
    });
}

function RequeueSingleMerchant(merchantNumber) {
    var actionUrl = rootURL + "risk_MCF/rm_MCF_DQNextQReportPopup.aspx/UpdateRequeuedMerchant";
    $.ajax({
        type: "POST",
        async: false,
        url: actionUrl,
        data: '{"merchantNumber":"' + merchantNumber + '","applyFilteredId":"' + applyFilteredId + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
        }
    });
}

function AddOrRemoveWorkQueue(url) {
    var actionUrl = rootURL + "risk_MCF/rm_MCF_DQNextQReportPopup.aspx/GetWorkQueueAssignment";
    $.ajax({
        type: "POST",
        async: false,
        url: actionUrl,
        data: '{"applyFilteredId":"' + applyFilteredId + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            ShowPopupModal(url, 'auto');
        }
    });

    return false;
}

function OpenFlagColorTable() {
    ShowPopupModal("rm_MCF_ColorLegend.aspx", 'auto')
    return false;
}

function RebindBarometer_NextQueue() {
    $get(uxRebindBarometerID).click();
}

function doOpenCustomViewPopup(url) {
    registerCloseCustomViewModalEvent();
    return doOpenNewPopup(url);
}


var currentWork = {
    state: -1,
    unCheckLastItem: false,
    listDis: []
}

function ajaxRequestStart(sender, args) {
    if (args.get_eventTarget().indexOf('uxExporter') != -1) {
        args.set_enableAjax(false);
    }
}

function reloadRainbowReport() {
    var grid = $find(detectionQueueRainbowReport_uxReportGrid).get_masterTableView();
    grid.rebind();
}

function setTabActive(isGridView) {
    var isGridViewMode = isGridView == '1';
    $(".view-data-options > .btn-option").removeClass("active");
    if (isGridViewMode) {
        $(".view-data-options > .btn-option-gridview").addClass("active");
    } else {
        $(".view-data-options > .btn-option-cardview").addClass("active");
    }
}
function setExportOption(isAllFields) {
    $("#" + detectionQueue_hddExportOption).val(isAllFields);
}

function doOpenNewPopup(encodeURL) {
    return parent.ShowPopupModal(encodeURL, 'auto');
}
parent.selectMerchantWorked = function (url) {
    $("#ucSelectMerchantWorked").attr("href", url);
    document.getElementById("ucSelectMerchantWorked").click()
}


function changeWorked(e) {
    if ($(e).parents(".disposition-content").find("input").prop("checked")) {
        $(e).parents(".disposition-content").find("input").prop("checked", false);
        $(e).parents(".disposition-content").find("input").change();
    } else {
        $(e).parents(".disposition-content").find("input").prop("checked", true);
        $(e).parents(".disposition-content").find("input").change();
    }
}

function changeWorkedState(e, state) {
    var targetId = state == 0 ? $(e).parent().attr("id") :
        $(e).parents(".disposition-dialog").attr("targetid");
    if ($(e).attr("data-wip") != 3) {
        getCurrentState(e, state);
        switch (state) {
            case 3:
                selectDisposition(e, targetId);
                break;
            default:
                break;
        }
    }

}
function returnCurrentWork(targetId, message) {
    var state = parseInt(currentWork.state);
    $("#" + targetId).removeClass("wk-selected");

    switchWorkStatus(targetId, state);

    $(".disposition-dialog[targetid='" + targetId + "']").find("ul").find("li").each(function (index) {
        var val = $(this).find("input").val();
        if (currentWork.listDis.indexOf(val) != -1) {
            $(this).find("input").prop("checked", true);
        }
    });
    if (state == 1) {
        $(".disposition-dialog[targetid='" + targetId + "'] .rd-work").prop("checked", true);
        $(".disposition-dialog[targetid='" + targetId + "'] .rd-dis").prop("checked", false);
    } else if (state == 2) {
        $(".disposition-dialog[targetid='" + targetId + "'] .rd-work").prop("checked", false);
        $(".disposition-dialog[targetid='" + targetId + "'] .rd-dis").prop("checked", true);
    } else {
        $(".disposition-dialog[targetid='" + targetId + "'] .rd-work").prop("checked", false);
        $(".disposition-dialog[targetid='" + targetId + "'] .rd-dis").prop("checked", false);
    }

    if (message) {
        $("#" + hddMessage_ClientID).val(message.toString());
    }
    $get(uxOpenWarning_ClientID).click();
}

function switchWorkStatus(targetId, WorkStateID) {

    var element = $("#" + targetId);

    switch (WorkStateID) {
        case 0:
            element.find(".work-status").eq(0).attr("class", "work-status");
            element.find(".work-ico").eq(0).attr("class", "work-ico");
            element.attr("data-state", 0);
            $(".disposition-dialog[targetid='" + targetId + "']").find("ul").find("li").each(function (index) {
                $(this).find("input").prop("checked", false);

            });
            break;
        case 1:
            element.find(".work-status").eq(0).attr("class", "work-status txt-wip");
            element.find(".work-ico").eq(0).attr("class", "work-ico icon-warning");
            element.attr("data-state", 1);
            break;
        case 2:
            element.find(".work-status").eq(0).attr("class", "work-status txt-wkd");
            element.find(".work-ico").eq(0).attr("class", "work-ico icon-check");
            element.attr("data-state", 2);
            break;
        default:
            break;
    }
}

function ChangeMerchantWorked(e, targetId) {
    if ($(e).attr("data-state") != "0") {
        $(e).parent().removeClass("wk-selected");
        switchWorkStatus(targetId, 0);
        removeDefault(targetId);
    } else {
        switchWorkStatus(targetId, 2);
        $(e).parent().addClass("wk-selected");
    }
    removeDefault($(e).parent().attr("id"));
}

function workPopBeforeShow(sender, args) {
    sender.updateLocation();
    sender._adjustCallout();
}

function workPopShow(sender, args) {
    setDispositionByTargetID(sender._targetControlID);
}

function wipOtherShow(sender, args) {
    var targetId = $(sender._element)[0].id;
    $("#RadToolTipWrapper_" + targetId).addClass("active");
}
function wipOtherHideShow(sender, args) {
    var targetId = $(sender._element)[0].id;
    $("#RadToolTipWrapper_" + targetId).removeClass("active");
}

function openWarning(message) {
    parent.ShowPopupModal(rootURL + 'Risk_MCF/rm_MCF_WorkWarning.aspx?' + message, 'auto');
}

function setDispositionByTargetID(targetId) {
    if ($("#" + targetId).hasClass("wk-selected")) {
        var workingMerchantID = $("#" + targetId).attr("workingMerchantID");
        var reportDate = $("#" + targetId).attr("reportdate");
        $.when(checkDisposition(workingMerchantID, reportDate)).done(function (data) {
            rebindDispositionValue(JSON.parse(data.d).Table, targetId);

            $(".disposition-dialog[targetid='" + targetId + "']").parent().
                parent().find("ul").find("li").each(function (index) {
                    if ($(this).find("input").attr("isdefault")) {
                        $(this).find("input").prop("checked", true);
                    }
                });
        });
        $(".disposition-dialog[targetid='" + targetId + "'] .rd-dis").prop("checked", true);
        $(".disposition-dialog[targetid='" + targetId + "']").find("ul").find("li").each(function (index) {
            if ($(this).find("input").attr("isdefault")) {
                $(this).find("input").prop("checked", true);
            }
        });
        $("#" + targetId).removeClass("wk-selected");
    }
}

function removeDefault(targetId) {
    $(".disposition-dialog[targetid='" + targetId + "'] .rd-work").prop("checked", false);
    $(".disposition-dialog[targetid='" + targetId + "'] .rd-dis").prop("checked", false);
    $(".disposition-dialog[targetid='" + targetId + "']").find("ul").find("li").each(function (index) {
        if ($(this).find("input").attr("isdefault")) {
            $(this).find("input").prop("checked", false);
        }
    });
}

function rebindDispositionValue(dataSource, targetId) {
    var element;
    dataSource.forEach(function (item) {
        if (item) {
            element = $(".disposition-dialog[targetid='" + targetId + "']").find("ul").
                find("li").find("span").filter(function () {
                    return $(this).text() == item[1];
                })
            if (element.length > 0) {
                if (item[2]) {
                    element.parent().find("input").attr("isdefault", "true");

                } else {
                    element.parent().find("input").removeAttr("isdefault");
                }
            }
        }
    })
}

function selectDisposition(e, targetId) {
    if ($(e).parents(".disposition-dialog").find("ul").find("li").find("input").filter(":checked").length > 0) {
        switchWorkStatus($(e).parents(".disposition-dialog").attr("targetid"), 2);
        currentWork.unCheckLastItem = false;
        $(e).parents(".disposition-dialog").find(".rd-dis").last().prop("checked", true);
    } else {
        currentWork.unCheckLastItem = true;
        switchWorkStatus($(e).parents(".disposition-dialog").attr("targetid"), 0);
        $(e).parents(".disposition-dialog").find(".rd-dis").last().prop("checked", false);
    }
}

function getCurrentState(e, state) {
    var targetId = state == 0 ? $(e).parent().attr("id") :
        $(e).parents(".disposition-dialog").attr("targetid");

    currentWork.listDis = [];
    var element = $("#" + targetId);

    currentWork.state = element.attr("data-state");
    $(".disposition-dialog[targetid='" + targetId + "']").find("ul").find("li")
        .find("input").filter(":checked").each(function (index) {
            currentWork.listDis.push($(this).val());
        });

    if (state == 3) {
        var curentValue = $(e).val();
        var currentChecked = $(e).is(":checked");
        if (!currentChecked) {
            currentWork.listDis.push(curentValue);
        } else {
            currentWork.listDis.splice(currentWork.listDis.indexOf(curentValue), 1);
        }
    }
}

function checkDisposition(WorkingMerchantID, reportDate) {
    var url = "rm_MCF_DQNextQWebMethod.aspx/CheckDisposition";
    return $.ajax({
        type: "POST",
        url: url,
        data: '{"Id":"' + WorkingMerchantID + '",reportDate:"' + reportDate + '"}',
        contentType: "application/json; charset=utf-8",
    });
}

function updateDisposition(cycleid, parentCycleID, merchantNumber, assignmentID, reportDate, dispositionList, newState) {
    var url = "rm_MCF_DQNextQWebMethod.aspx/UpdateDisposition";
    return $.ajax({
        type: "POST",
        url: url,
        data: '{"dispositionList":"' + dispositionList +
            '",workStateID:"' + newState +
            '",feWorkStateID:"' + currentWork.state +
            '",cycleID:"' + cycleid +
            '",ParentCycleID:"' + parentCycleID +
            '",reportDate:"' + reportDate +
            '",assignmentID:"' + assignmentID +
            '",merchantNumber:"' + merchantNumber +
            '",viewCode:"' + "NQ" + '"}',
        contentType: "application/json; charset=utf-8",
    });
}

function ChangeWorkedStatusOption(obj, option) {
    $('#uxWorkedNotWorked .btn-work-js').each(function () {
        if (this == obj) {
            if ($(obj).hasClass('btn-work-selected')) {
                $(obj).removeClass('btn-work-selected');
                option = '';
            } else {
                $(obj).addClass('btn-work-selected');
            }
        } else {
            $(this).removeClass('btn-work-selected');
        }
    });
    $get(rm_MCF_DetectionQueueRainbowReport_filterWorkingStatus).value = option;
    $get(rm_MCF_DetectionQueueRainbowReport_btnFilterWorkingStatus).click();
}

function refreshDataEvent() {
    $get(rm_MCF_DetectionQueueRainbowReport_btnRefreshRainbow).click();
}

function closeCurrentDisposition() {
    var currentToolTip = Telerik.Web.UI.RadToolTip.getCurrent();
    if (currentToolTip) {
        var element = currentToolTip.get_element();
        var id = ($(element)[0].id);
        $("#RadToolTipWrapper_" + id).hide();
    }
}

function DoDismissAndNext() {
    $get(rm_DQNextQReportPopup_uxNextHide).click();
    //$('#' + rm_DQNextQReportPopup_uxNextHide).click();
}

function showNextQueueNodata(msg) {
    setTimeout(function () {
        var c = confirm(msg);
        if (true) {
            window.close();
        }
    }, 500);
}
function setDispositionAndNextAsync(cycleid, parentCycleID, merchantNumber, assignmentID, reportDate, dispositionList, newState, feState) {
    var url = "rm_MCF_DQNextQReportPopup.aspx/SetDispositionAndNextAsync";
    return $.ajax({
        type: "POST",
        url: url,
        data: '{"dispositionList":"' + dispositionList +
            '",workStateID:"' + newState +
            '",feWorkStateID:"' + feState +
            '",cycleID:"' + cycleid +
            '",ParentCycleID:"' + parentCycleID +
            '",reportDate:"' + reportDate +
            '",assignmentID:"' + assignmentID +
            '",merchantNumber:"' + merchantNumber +
            '",viewCode:"' + "NQ" + '"}',
        contentType: "application/json; charset=utf-8",
    });
}
function setDispositionAndNext(e) {
    var targetId = $(e).attr("id");
    var newState = 2;
    var element = $("#" + targetId);
    var cycleid = element.attr("cycleid");
    var parentCycleID = element.attr("parentcycleid");
    var merchantNumber = element.attr("merchantnumber");
    var assignmentID = element.attr("assignmentid");
    var reportDate = element.attr("reportdate");
    var feState = element.attr("feworkstate").trim() == "0" ? "1" : element.attr("feworkstate");
    var dispositionList = "";
    var dispositionArray = [];
    $(".disposition-dialog[targetid='" + targetId + "']").find("ul").find("li")
        .find("input").filter(":checked").each(function (index) {
            dispositionArray.push($(this).val());
        });
    dispositionList = dispositionArray.join();
    setDispositionAndNextAsync(cycleid, parentCycleID, merchantNumber, assignmentID, reportDate, dispositionList, newState, feState, true);
}


//$(document).ready(function () {
//    LoadAssignmentInfo()
//});

//function LoadAssignmentInfo() {
//    $('#' + rm_DQNextQReportPopup_uxReloadAssignment).click();
//}